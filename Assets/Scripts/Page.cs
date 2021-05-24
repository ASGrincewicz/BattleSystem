using System;
using UnityEngine;

namespace Veganimus.BattleSystem
{
    ///<summary>
    ///@author
    ///Aaron Grincewicz
    ///</summary>
    [CreateAssetMenu(menuName ="New Page")]
    public class Page: ScriptableObject,IComparable<Page>
    {
        [Multiline(5)]
        public string content;
        public int pageNumber;

        public Page() { }

        public Page(string content) => this.content = content;

        public int CompareTo(Page other)
        {
            if (this.pageNumber > other.pageNumber)
                return 1;
            else if (this.pageNumber < other.pageNumber)
                return -1;
            else
                return 0;
        }
    }
}
