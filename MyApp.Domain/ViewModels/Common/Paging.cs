using Microsoft.EntityFrameworkCore;

namespace MyApp.Domain.ViewModels.Common
{
    public class BasePaging<T>
    {
        #region Properties

        public int Page { get; set; }
        public int PageCount { get; set; }
        public int AllEntitiesCount { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int TakeEntity { get; set; }
        public int SkipEntity { get; set; }
        public int HowManyShowPageAfterAndBefore { get; set; }
        public List<T> Entities { get; set; }

        #endregion

        #region Constructor

        public BasePaging()
        {
            Page = 1;
            TakeEntity = 4;
            HowManyShowPageAfterAndBefore = 5;
            Entities = new List<T>();
        }

        #endregion

        #region Methods - Paging Logic

        public async Task<BasePaging<T>> Paging(IQueryable<T> queryable)
        {
            TakeEntity = TakeEntity;

            var allEntitiesCount = await queryable.CountAsync();

            var pageCount = CalculatePageCount(allEntitiesCount);
            Page = NormalizePage(pageCount);
            AllEntitiesCount = allEntitiesCount;
            SkipEntity = (Page - 1) * TakeEntity;
            SetPageRange(pageCount);

            Entities = await queryable.Skip(SkipEntity).Take(TakeEntity).ToListAsync();

            return this;
        }

        public BasePaging<T> Paging()
        {
            var allEntitiesCount = Entities.Count;

            var pageCount = CalculatePageCount(allEntitiesCount);
            Page = NormalizePage(pageCount);
            AllEntitiesCount = allEntitiesCount;
            SetPageRange(pageCount);
            return this;
        }

        #endregion

        #region Methods - Helper Functions

        private int CalculatePageCount(int entityCount)
        {
            try
            {
                return Convert.ToInt32(Math.Ceiling(entityCount / (double)TakeEntity));
            }
            catch (Exception)
            {
                return 0; // Handle the case where division fails
            }
        }

        private int NormalizePage(int pageCount)
        {
            if (Page > pageCount) return pageCount;
            return Page <= 0 ? 1 : Page;
        }

        private void SetPageRange(int pageCount)
        {
            StartPage = Page - HowManyShowPageAfterAndBefore <= 0 ? 1 : Page - HowManyShowPageAfterAndBefore;
            EndPage = Page + HowManyShowPageAfterAndBefore > pageCount ? pageCount : Page + HowManyShowPageAfterAndBefore;
            PageCount = pageCount;
        }

        #endregion

        #region Methods - View Model

        public PagingViewModel GetCurrentPaging()
        {
            return new PagingViewModel
            {
                EndPage = EndPage,
                Page = Page,
                StartPage = StartPage,
                PageCount = PageCount
            };
        }

        #endregion
    }

    #region PagingViewModel
    public class PagingViewModel
    {
        public int Page { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int PageCount { get; set; }
    }
    #endregion

}
