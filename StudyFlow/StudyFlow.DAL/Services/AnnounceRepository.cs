﻿using Microsoft.EntityFrameworkCore;
using StudyFlow.DAL.Data;
using StudyFlow.DAL.Entities;
using StudyFlow.DAL.Entities.Helper;
using StudyFlow.DAL.Interfaces;
using StudyFlow.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyFlow.DAL.Repositories
{
    public class AnnounceRepository : Repository<Announce>, IAnnounceRepository
    {
        private readonly DataContext _context;

        public AnnounceRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Announce> AddAnnounceAsync(Announce announce)
        {
            await _context.Set<Announce>().AddAsync(announce);
            return announce;
        }

        // Implementación de eliminación lógica manteniendo el nombre `DeleteAnnounceAsync`
        public async Task<bool> DeleteAnnounceAsync(Guid id)
        {
            var announce = await _context.Set<Announce>().FindAsync(id);
            if (announce == null)
            {
                return false;
            }

            announce.IsDeleted = true;
            _context.Set<Announce>().Update(announce);
            return true;
        }

        public async Task<bool> UpdateAnnounceAsync(Announce announce)
        {
            var existingAnnounce = await _context.Set<Announce>().FindAsync(announce.Id);
            if (existingAnnounce == null || existingAnnounce.IsDeleted)
            {
                return false;
            }

            _context.Entry(existingAnnounce).CurrentValues.SetValues(announce);
            return true;
        }

        public async Task<IEnumerable<Announce>> GetAllAnnouncesAsync()
        {
            return await _context.Set<Announce>()
                .AsNoTracking()
                .Where(a => !a.IsDeleted)
                .Include(a => a.User)
                .Include(a => a.Course)
                .ToListAsync();
        }

        public async Task<PaginationResult<Announce>> GetAllAnnouncesAsync(Pagination pagination)
        {
            var query = _context.Set<Announce>()
                .AsNoTracking()
                .Where(a => !a.IsDeleted)
                .Include(a => a.User)
                .Include(a => a.Course)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                query = query.Where(a => a.Title.Contains(pagination.Filter, StringComparison.OrdinalIgnoreCase));
            }

            int totalRecords = await query.CountAsync();
            var items = await query
                .Skip((pagination.Page - 1) * pagination.RecordsNumber)
                .Take(pagination.RecordsNumber)
                .ToListAsync();

            return new PaginationResult<Announce>
            {
                ListResult = items,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pagination.RecordsNumber),
                Pagination = pagination
            };
        }

        public async Task<IEnumerable<Announce>> GetAnnouncesByUserIdAsync(Guid userId)
        {
            return await _context.Set<Announce>()
                .AsNoTracking()
                .Where(a => !a.IsDeleted && a.UserId == userId)
                .Include(a => a.Course)
                .ToListAsync();
        }

        public async Task<PaginationResult<Announce>> GetAnnouncesByUserIdAsync(Guid userId, Pagination pagination)
        {
            var query = _context.Set<Announce>()
                .AsNoTracking()
                .Where(a => !a.IsDeleted && a.UserId == userId)
                .Include(a => a.Course)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                query = query.Where(a => a.Title.Contains(pagination.Filter, StringComparison.OrdinalIgnoreCase));
            }

            int totalRecords = await query.CountAsync();
            var items = await query
                .Skip((pagination.Page - 1) * pagination.RecordsNumber)
                .Take(pagination.RecordsNumber)
                .ToListAsync();

            return new PaginationResult<Announce>
            {
                ListResult = items,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pagination.RecordsNumber),
                Pagination = pagination
            };
        }

        public async Task<Announce?> GetAnnounceWithDetailsAsync(Guid id)
        {
            return await _context.Set<Announce>()
                .AsNoTracking()
                .Where(a => !a.IsDeleted)
                .Include(a => a.User)
                .Include(a => a.Course)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Announce>> GetAnnouncesByCourseIdAsync(Guid courseId)
        {
            return await _context.Set<Announce>()
                .AsNoTracking()
                .Where(a => !a.IsDeleted && a.Course.Id == courseId)
                .Include(a => a.User)
                .ToListAsync();
        }

        public async Task<PaginationResult<Announce>> GetAnnouncesByCourseIdAsync(Guid courseId, Pagination pagination)
        {
            var query = _context.Set<Announce>()
                .AsNoTracking()
                .Where(a => !a.IsDeleted && a.Course.Id == courseId)
                .Include(a => a.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                query = query.Where(a => a.Title.Contains(pagination.Filter, StringComparison.OrdinalIgnoreCase));
            }

            int totalRecords = await query.CountAsync();
            var items = await query
                .Skip((pagination.Page - 1) * pagination.RecordsNumber)
                .Take(pagination.RecordsNumber)
                .ToListAsync();

            return new PaginationResult<Announce>
            {
                ListResult = items,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pagination.RecordsNumber),
                Pagination = pagination
            };
        }

        public async Task<IEnumerable<Announce>> GetAllAnnouncesWithDetailsAsync()
        {
            return await _context.Set<Announce>()
                .AsNoTracking()
                .Where(a => !a.IsDeleted)
                .Include(a => a.User)
                .Include(a => a.Course)
                .ToListAsync();
        }

        public async Task<PaginationResult<Announce>> GetAllAnnouncesWithDetailsAsync(Pagination pagination)
        {
            var query = _context.Set<Announce>()
                .AsNoTracking()
                .Where(a => !a.IsDeleted)
                .Include(a => a.User)
                .Include(a => a.Course)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                query = query.Where(a => a.Title.Contains(pagination.Filter, StringComparison.OrdinalIgnoreCase));
            }

            int totalRecords = await query.CountAsync();
            var items = await query
                .Skip((pagination.Page - 1) * pagination.RecordsNumber)
                .Take(pagination.RecordsNumber)
                .ToListAsync();

            return new PaginationResult<Announce>
            {
                ListResult = items,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pagination.RecordsNumber),
                Pagination = pagination
            };
        }
    }
}