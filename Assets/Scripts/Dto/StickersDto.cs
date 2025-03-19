using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Dto
{
    public class StickersDto
    {
        public StickerDto[] stickers = new StickerDto[]
        {
            new StickerDto { Date = DateTime.UtcNow, isAlreadyUnpacked = false, stickerId = 1 },
            new StickerDto { Date = DateTime.UtcNow.AddDays(2), isAlreadyUnpacked = true, stickerId = 2 },
            new StickerDto { Date = DateTime.UtcNow.AddDays(-2), isAlreadyUnpacked = true, stickerId = 3 }
        };


        public class StickerDto
        {
            public bool isAlreadyUnpacked;
            public DateTime Date; // mischien kan hier beter van "het is unlocked" gemaakt worden, en dan met een reset methode, die controlerd om de tijd of het nog steeds unlocked is of niet en dan de bool weer op false zet of niet
            public int stickerId;
        }
    }
}
