﻿using Hotel_Management.Helpers;
using Hotel_Management.Models;
using Hotel_Management.Areas.Admin.ViewModels;

namespace Hotel_Management.Areas.Admin.Services.Interfaces
{
    public interface IRoomService
    {
        /// <summary>
        /// Lấy danh sách phòng
        /// </summary>
        /// <param name="searchTerm">Tìm kiếm theo số phòng, loại phòng, mô tả</param>
        /// <param name="roomType">Lọc theo loại phòng</param>
        /// <param name="status">Lọc theo trạng thái phòng</param>
        /// <param name="minPrice">Lọc theo giá tối thiểu</param>
        /// <param name="maxPrice">Lọc theo giá tối đa</param>
        /// <param name="pageIndex">Trang hiện tại</param>
        /// <param name="pageSize">Tổng số thành phần</param>
        /// <returns>Paginated list of room view models</returns>
        Task<PaginatedList<RoomVM>> GetAllAsync(
            string searchTerm = "", 
            string roomType = "", 
            string status = "", 
            decimal? minPrice = null, 
            decimal? maxPrice = null, 
            int pageIndex = 1, 
            int pageSize = 20);

        /// <summary>
        /// Lấy thông tin phòng theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<RoomVM> GetByIdAsync(int id);

        /// <summary>
        /// Lấy danh sách các loại phòng
        /// </summary>
        /// <returns>List of unique room types</returns>
        Task<List<string>> GetAllRoomTypesAsync();

        /// <summary>
        /// Thêm mới phòng
        /// </summary>
        /// <param name="roomVM">Thông tin phòng</param>
        /// <returns>True/False</returns>
        Task<bool> CreateAsync(RoomVM roomVM);

        /// <summary>
        /// Cập nhật thông tin phòng
        /// </summary>
        /// <param name="roomVM">Thông tin phòng</param>
        /// <returns>True/False</returns>
        Task<bool> UpdateAsync(RoomVM roomVM);

        /// <summary>
        /// Xóa phòng theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int id);
    }
}
