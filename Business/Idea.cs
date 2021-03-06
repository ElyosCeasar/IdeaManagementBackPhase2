﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObject.Common;
using DataTransferObject.Idea;


namespace Business
{
    public class Idea
    {
        private readonly DataAccess.Query.IdeaQ _Repository;
        //-------------------------------------------------------------------------------------------------
        public Idea()
        {
            _Repository = new DataAccess.Query.IdeaQ();
        }
        //-------------------------------------------------------------------------------------------------
        public IEnumerable<IdeaForShowDto> GetAllIdea()
        {
            return _Repository.GetAllIdea();
        }
        //-------------------------------------------------------------------------------------------------
        public IdeaDetailForShowDto GetSpecificIdea(int ideaId)
        {
            return _Repository.GetSpecificIdea(ideaId);
        }
        //-------------------------------------------------------------------------------------------------
        public Result SendNewIdea(NewIdeaDto idea)
        {
            return _Repository.SendNewIdea(idea);
        }
        //-------------------------------------------------------------------------------------------------
        public Result EditIdea(int ideaId, ChangedIdeaDto idea)
        {
            if (_Repository.IsIdeaLocked(ideaId))
            {
                return new Result()
                {
                    value = false,
                    content = "ایده توسط کمیته بررسی شده و یا وجود ندارد و قابل تغییر نیست"
                };
            }else
            return _Repository.EditIdea(ideaId, idea);
        }

        //-------------------------------------------------------------------------------------------------
        public Result VoteToIdea(IdeaPointDto vote)
        {
            if (vote.POINT < 0)
            {
                vote.POINT = -1;
            }else if (vote.POINT > 0)
            {
                vote.POINT = 1;
            }
            else
            {
                vote.POINT = 0;
            }
            if (_Repository.IsIdeaLocked(vote.IDEA_ID))
            {
                
                return new Result()
                {
                    value = false,
                    content = "ایده توسط کمیته بررسی شده و یا وجود ندارد و قابل رای دهی نیست"
                };
            }
            else
                return _Repository.VoteToIdea(vote);
        }

        //-------------------------------------------------------------------------------------------------
        public IEnumerable<IdeaDto> GetIdeasTop10All()
        {
            return _Repository.GetIdeasTop10All();
        }
        
        //-------------------------------------------------------------------------------------------------
        public IEnumerable<IdeaDto> GetIdeasTop10CurrentMonth()
        {
            return _Repository.GetIdeasTop10CurrentMonth();
        }
        
        //-------------------------------------------------------------------------------------------------
        public IEnumerable<IdeaDto> GetIdeasTop10CurrentWeek()
        {
            return _Repository.GetIdeasTop10CurrentWeek();
        }
    }
}
