using System.Linq;
using UnityEngine;

namespace MH.DataSystem
{
    [System.Serializable]
    public class ItemExam : IIdentifiable
    {
        public string ID;
        public string Name;
        public float[] Stats;
        public int RequireLevel;
        public string Id { get => ID; }
    } 

    public class DataSystemTest : MonoBehaviour
    {

        public ItemExam[] ItemExams;

        public void Start()
        {
            var loader = new JsonLoader<ItemExam>();
            var itemProvider = new DataProvider<ItemExam>("Item.txt", loader);

            ItemExams = itemProvider.GetAll().ToArray();
        }
    }
}
