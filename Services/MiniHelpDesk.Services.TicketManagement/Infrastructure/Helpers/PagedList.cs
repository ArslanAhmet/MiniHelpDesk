﻿//using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniHelpDesk.Services.TicketManagement.Infrastructure.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public bool HasPrevious { get {  return (CurrentPage > 1); } }
        public bool HasNext { get { return (CurrentPage < TotalPages); } }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count/(double)pageSize);
            AddRange(items);
        }

        //public async static Task<PagedList<T>>  Create(IQueryable<T> source, int pageNumber, int pageSize)
        //{
        //    int count = source.Count();
        //    var items = await source.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync();
        //    return new PagedList<T>(items, count, pageNumber, pageSize);
        //}
    }
}
