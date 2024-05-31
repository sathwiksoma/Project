using System.ComponentModel.DataAnnotations;

namespace HotPotProject.Models
{
    public class State
    {
        [Key]
        public int StateId { get; set; }
        public string Name { get; set; }

        public State()
        {

        }

        public State(int stateId, string name)
        {
            StateId = stateId;
            Name = name;
        }

        public State(string name)
        {
            Name = name;
        }
    }
}
