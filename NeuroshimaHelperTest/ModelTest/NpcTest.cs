using NeuroshimaDB.Models;
using System.Diagnostics;

namespace NeuroshimaHelperTest.ModelTest
{
    [TestClass]
    public class NpcTest
    {
        [TestMethod, TestCategory("Npc")]
        public void ContructorTest()
        {
            Npc npc;
            for (int i = 0; i < 10; i++)
            {
                npc = new(1, "Chemik");
                foreach (var item in npc.Skills)
                    Debug.Write($"{item.Key}: {item.Value}; ");
                Debug.WriteLine("");
            }
        }
    }
}