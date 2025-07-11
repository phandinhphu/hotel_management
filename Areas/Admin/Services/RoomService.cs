﻿using Microsoft.EntityFrameworkCore;
using Hotel_Management.Helpers;
using Hotel_Management.Models;
using Hotel_Management.Areas.Admin.ViewModels;
using Hotel_Management.Areas.Admin.Services.Interfaces;

namespace Hotel_Management.Areas.Admin.Services
{
    public class RoomService : IRoomService
    {
        private readonly HotelManagementContext _context;
        private readonly ImageHelper _imageHelper;
        private readonly IImageServices _cloudinaryImageService;

        public RoomService(HotelManagementContext context, ImageHelper imageHelper, IImageServices cloudinaryImageService)
        {
            _context = context;
            _imageHelper = imageHelper;
            _cloudinaryImageService = cloudinaryImageService;
        }

        #region Async Methods

        public async Task<PaginatedList<RoomVM>> GetAllAsync(
            string searchTerm = "", 
            string roomType = "", 
            string status = "", 
            decimal? minPrice = null, 
            decimal? maxPrice = null, 
            int pageIndex = 1, 
            int pageSize = 20)
        {
            var query = _context.Rooms.AsNoTracking();
            
            // Search
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim().ToLower();
                query = query.Where(r => 
                    (r.RoomNumber != null && r.RoomNumber.ToLower().Contains(searchTerm)) ||
                    (r.Type != null && r.Type.ToLower().Contains(searchTerm)) ||
                    (r.Description != null && r.Description.ToLower().Contains(searchTerm))
                );
            }
            // Filters
            if (!string.IsNullOrWhiteSpace(roomType))
            {
                query = query.Where(r => r.Type == roomType);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(r => r.Status == status);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(r => r.Price >= minPrice);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(r => r.Price <= maxPrice);
            }
            query = query.OrderBy(r => r.Id);

            var mappedQuery = query.Select(r => new RoomVM
            {
                ID = r.Id,
                HotelId = r.HotelId ?? 0,
                RoomNumber = r.RoomNumber ?? "",
                Type = r.Type ?? "",
                Description = r.Description ?? "",
                Price = r.Price ?? 0,
                Status = r.Status ?? "",
                Capacity = r.Capacity ?? 0,
                Image = r.Image ?? "",
                Images = r.Roomimages.Select(ri => ri.ImageUrl).ToList()
            });
            
            return await PaginatedList<RoomVM>.Create(
                mappedQuery,
                pageIndex,
                pageSize
            );
        }

        public async Task<RoomVM> GetByIdAsync(int id)
        {
            var room = await _context.Rooms
                .AsNoTracking()
                .Where(r => r.Id == id)
                .Select(r => new RoomVM
                {
                    ID = r.Id,
                    HotelId = r.HotelId ?? 0,
                    RoomNumber = r.RoomNumber ?? "",
                    Type = r.Type ?? "",
                    Description = r.Description ?? "",
                    Price = r.Price ?? 0,
                    Status = r.Status ?? "",
                    Capacity = r.Capacity ?? 0,
                    Image = r.Image ?? "",
                    Images = r.Roomimages.Select(ri => ri.ImageUrl).ToList()
                }).FirstOrDefaultAsync();
            if (room == null)
            {
                throw new KeyNotFoundException($"Room with ID {id} not found.");
            }
            return room;
        }

        public async Task<List<string>> GetAllRoomTypesAsync()
        {
            var types = await _context.Rooms
                .AsNoTracking()
                .Where(r => r.Type != null && r.Type != "")
                .Select(r => r.Type!)
                .Distinct()
                .ToListAsync();

            if (types == null || types.Count == 0)
            {
                throw new InvalidOperationException("No room types found.");
            }

            return types;
        }

        public async Task<bool> CreateAsync(RoomVM roomVM)
        {
            var existingRoom = await _context.Rooms
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RoomNumber == roomVM.RoomNumber);
            if (existingRoom != null)
            {
                return false;
            }
            // Lưu ảnh đại diện
            if(roomVM.ImagetFile != null)
            {
                var imgUrl = await _cloudinaryImageService.UploadImageAsync(roomVM.ImagetFile, "room");
                if (string.IsNullOrEmpty(imgUrl))
                {
                    return false; // Không thể lưu ảnh
                }
                roomVM.Image = imgUrl;
            }
            // Map ViewModel to entity
            var room = new Room
            {
                HotelId = roomVM.HotelId,
                RoomNumber = roomVM.RoomNumber,
                Type = roomVM.Type,
                Description = roomVM.Description,
                Price = roomVM.Price,
                Status = roomVM.Status,
                Capacity = roomVM.Capacity,
                Image = roomVM.Image
            };

            // Add to database
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            //Lưu ảnh bổ sung
            if (roomVM.ImageFiles != null)
            {
                List<string> imageUrls = await _cloudinaryImageService.UploadImagesAsync(roomVM.ImageFiles, "room");
                foreach (var imageUrl in imageUrls)
                {
                    var roomImage = new Roomimage
                    {
                        RoomId = room.Id,
                        ImageUrl = imageUrl
                    };
                    _context.Roomimages.Add(roomImage);
                }
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> UpdateAsync(RoomVM roomVM)
        {
            var existingRoom = await _context.Rooms
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == roomVM.ID);
            if (existingRoom == null)
            {
                return false;
            }

            // Nếu có file ảnh mới, lưu và xóa ảnh cũ nếu có
            if (roomVM.ImagetFile != null && roomVM.ImagetFile.Length > 0)
            {
                // Xóa ảnh cũ nếu có
                if (!string.IsNullOrEmpty(existingRoom.Image))
                {
                    await _cloudinaryImageService.DeleteImageAsync(existingRoom.Image);
                }

                // Lưu ảnh mới
                var imgUrl = await _cloudinaryImageService.UploadImageAsync(roomVM.ImagetFile, "room");
                if (string.IsNullOrEmpty(imgUrl))
                {
                    return false; // Không thể lưu ảnh mới
                }
                roomVM.Image = imgUrl; // Cập nhật ảnh mới vào ViewModel
            }
            else
            {
                roomVM.Image = existingRoom.Image;
            }

            // Map ViewModel to entity
            existingRoom.RoomNumber = roomVM.RoomNumber;
            existingRoom.Type = roomVM.Type;
            existingRoom.Description = roomVM.Description;
            existingRoom.Price = roomVM.Price;
            existingRoom.Status = roomVM.Status;
            existingRoom.Capacity = roomVM.Capacity;
            existingRoom.Image = roomVM.Image;
            // Update in database
            _context.Rooms.Update(existingRoom);
            await _context.SaveChangesAsync();

            // Xử lý ảnh bổ sung
            if (roomVM.ImageFiles != null && roomVM.ImageFiles.Count > 0)
            {
                var existingImages = await _context.Roomimages.Where(ri => ri.RoomId == existingRoom.Id).ToListAsync();
                if (existingImages.Any())
                {
                    foreach (var img in existingImages)
                    {
                        await _cloudinaryImageService.DeleteImageAsync(img.ImageUrl); // hàm delete phía dưới
                        _context.Roomimages.Remove(img);
                    }
                }
                // Lưu các ảnh mới
                List<string> imageUrls = await _cloudinaryImageService.UploadImagesAsync(roomVM.ImageFiles, "room");
                foreach (var imageUrl in imageUrls)
                {
                    var roomImage = new Roomimage
                    {
                        RoomId = roomVM.ID,
                        ImageUrl = imageUrl
                    };
                    _context.Roomimages.Add(roomImage);
                }
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingRoom = await _context.Rooms.FindAsync(id);
            if (existingRoom == null)
            {
                return false;
            }

            // Check if room has related bookings
            var hasBookings = await _context.BookingsRoomDetails.AnyAsync(b => b.RoomId == id);
            if (hasBookings)
            {
                throw new InvalidOperationException("Không thể xóa phòng này vì đã có người đặt. Hãy hủy các đặt phòng trước.");
            }

            // Xóa ảnh nếu có
            if (!string.IsNullOrEmpty(existingRoom.Image))
            {
                await _cloudinaryImageService.DeleteImageAsync(existingRoom.Image);
            }
            // Xóa ảnh bổ sung
            var existingImages = await _context.Roomimages.Where(ri => ri.RoomId == existingRoom.Id).ToListAsync();
            if (existingImages.Any())
            {
                foreach (var img in existingImages)
                {
                    await _cloudinaryImageService.DeleteImageAsync(img.ImageUrl);
                    _context.Roomimages.Remove(img);
                }
            }
            _context.Rooms.Remove(existingRoom);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion
    }
}
