using System;
using System.Collections.Generic;
using UnityEngine;

namespace Veganimus.BattleSystem
{
    [CreateAssetMenu(menuName = "new Chapter")]
    public class Chapter : ScriptableObject, IComparable<Chapter>
    {
        public List<Page> chapterPages = new List<Page>();
        public int chapterNumber;

        public Chapter() { }

        public Chapter(List<Page> pages)
        {
            foreach(Page page in pages)
            {
                chapterPages.Add(page);
            }
        }

        public int CompareTo(Chapter other)
        {
            if (this.chapterNumber > other.chapterNumber)
                return 1;
            else if (this.chapterNumber < other.chapterNumber)
                return -1;
            else
                return 0;
        }
    }

}
