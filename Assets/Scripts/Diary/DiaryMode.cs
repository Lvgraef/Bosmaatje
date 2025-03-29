using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary
{
    public abstract class DiaryMode
    {
        protected DiaryWriterManager _diaryWriter;

        public DiaryMode(DiaryWriterManager diaryWriter)
        {
            _diaryWriter = diaryWriter;
        }

        public abstract void Setup();
        public abstract void HandleSave();
        public abstract void HandleGoBack();
        public abstract void HandleClose();
    }

}
