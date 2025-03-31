using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.Build.Framework;

namespace Dto
{

    public class GetDiaryDataRequestDto
    {
        public List<DiaryReadDto> Diaries { get; set; }
    }

    public class DiaryReadDto
    {
        public DateTime date { get; set; }
        public string content { get; set; }
    }
}
