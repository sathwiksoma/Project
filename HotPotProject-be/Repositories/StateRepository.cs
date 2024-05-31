using HotPotProject.Context;
using HotPotProject.Exceptions;
using HotPotProject.Interfaces;
using HotPotProject.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPotProject.Repositories
{
    public class StateRepository : IRepository<int, String, State>
    {
        ApplicationTrackerContext _context;

        public StateRepository(ApplicationTrackerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Takes a State object and adds it to the database
        /// </summary>
        /// <param name="item"></param>
        /// <returns>A State object that has been added to the database</returns>
        public async Task<State> Add(State item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        /// <summary>
        /// Takes a primary key int and deletes the State record corresponding to it from the database
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A State object that has been deleted from the database</returns>
        /// <exception cref="StateNotFoundException"></exception>
        public async Task<State> Delete(int key)
        {
            var state = await GetAsync(key);
            if (state != null)
            {
                _context.States.Remove(state);
                _context.SaveChanges();
                return state;
            }
            throw new StateNotFoundException("No State found");
        }

        /// <summary>
        /// Takes a primary key int as parameter and returns the State object corresponding to it from the database
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A State object</returns>
        /// <exception cref="StateNotFoundException"></exception>
        public async Task<State> GetAsync(int key)
        {
            var states = await GetAsync();
            var state = states.FirstOrDefault(s => s.StateId == key);
            if (state != null)
                return state;
            throw new StateNotFoundException("No state found");
        }

        /// <summary>
        /// Returns a list of all the State entities from the database
        /// </summary>
        /// <returns>A List of State which contains all the State entities</returns>
        /// <exception cref="StateNotFoundException"></exception>
        public async Task<List<State>> GetAsync()
        {
            var states = _context.States.ToList();
            if (states != null || states.Count > 0)
                return states;
            throw new StateNotFoundException("No states available to show at the moment");
        }

        public Task<State> GetAsync(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Takes a State object and updates it's reference in the database
        /// </summary>
        /// <param name="item"></param>
        /// <returns>An updated State object</returns>
        /// <exception cref="StateNotFoundException"></exception>
        public async Task<State> Update(State item)
        {
            var state = await GetAsync(item.StateId);
            if (state != null)
            {
                _context.Entry<State>(item).State = EntityState.Modified;
                _context.SaveChanges();
                return item;
            }
            throw new StateNotFoundException("No state found");
        }
    }
}
