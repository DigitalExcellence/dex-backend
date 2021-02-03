/*
* Digital Excellence Copyright (C) 2020 Brend Smits
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as published
* by the Free Software Foundation version 3 of the License.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty
* of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
* See the GNU Lesser General Public License for more details.
*
* You can find a copy of the GNU Lesser General Public License
* along with this program, in the LICENSE.md file in the root project directory.
* If not, see https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using System;
using System.Linq;

namespace API.HelperClasses
{
    /// <summary>
    /// The interface for the Index Order helper.
    /// </summary>
    /// <typeparam name="T">Type T which should implement the IComparable interface.</typeparam>
    public interface IIndexOrderHelper<in T> where T : IComparable
    {
        /// <summary>
        /// This method check if a series of indexes is ascending and consecutive (order is not
        /// looked at).
        /// </summary>
        /// <param name="indexes">The indexes are the series of indexes that will get checked.</param>
        /// <param name="startingIndex">The starting index is the minimal value where the indexes should start from.</param>
        /// <returns>This method returns true or false. This method returns true when the series of indexes are ascending and consecutive
        /// and will return false whenever this is not the case.</returns>
        bool ValidateAscendingConsecutiveOrder(T[] indexes, T startingIndex);

    }

    /// <summary>
    /// The implementation for the Index Order helper.
    /// </summary>
    /// <typeparam name="T">Type T which should implement the IComparable interface.</typeparam>
    public class IndexOrderHelper<T> : IIndexOrderHelper<T> where T : IComparable
    {
        /// <summary>
        /// This method check if a series of indexes is ascending and consecutive (order is not
        /// looked at).
        /// </summary>
        /// <param name="indexes">The indexes are the series of indexes that will get checked.</param>
        /// <param name="startingIndex">The starting index is the minimal value where the indexes should start from.</param>
        /// <returns>This method returns true or false. This method returns true when the series of indexes are ascending and consecutive
        /// and will return false whenever this is not the case.</returns>
        public bool ValidateAscendingConsecutiveOrder(T[] indexes, T startingIndex)
        {
            // Check if there are no doubles
            if(indexes.Length !=
               indexes.Distinct()
                      .Count())
                return false;

            // Check if there is nothing smaller then the starting index
            if(indexes.Min().CompareTo(startingIndex) < 0)
                return false;

            // Check if the highest index is same as the length of the list.
            if(indexes.Max().CompareTo(indexes.Length) != 0)
                return false;

            return true;
        }

    }

}
