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
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Services.ExternalDataProviders.Helpers
{

    /// <summary>
    ///     The interface for the assembly helper.
    /// </summary>
    public interface IAssemblyHelper
    {

        /// <summary>
        ///     This method is responsible for retrieving all the types from the executing assembly folder.
        /// </summary>
        /// <returns>This method returns a collection of types.</returns>
        Type[] RetrieveTypesFromExecutingAssemblyFolderFiles();

        /// <summary>
        ///     This method is responsible to retrieve all types from the executing assembly folder by a specified
        ///     interface.
        /// </summary>
        /// <param name="interface">The interface that all returned types should inherit.</param>
        /// <returns>This method returns a collection of types that inherit the specified interface.</returns>
        Type[] RetrieveTypesFromExecutingAssemblyFolderFolderByInterface(Type @interface);

    }

    /// <summary>
    ///     The implementation of the assembly helper.
    /// </summary>
    public class AssemblyHelper : IAssemblyHelper
    {

        /// <summary>
        ///     This method is responsible for retrieving the location from the executing assembly.
        /// </summary>
        /// <returns>This method returns the location from the executing assembly.</returns>
        private Assembly GetExecutingAssembly()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            return executingAssembly;
        }

        /// <summary>
        ///     This method is responsible for retrieving all types from a specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly that will be searched for types.</param>
        /// <returns>This method returns a collection of types from an assemblyl.</returns>
        private Type[] RetrieveTypesFromAssembly(Assembly assembly)
        {
            return assembly.GetTypes();
        }

        /// <summary>
        ///     This method is responsible for retrieving all the types from the executing assembly folder.
        /// </summary>
        /// <returns>This method returns a collection of types.</returns>
        public Type[] RetrieveTypesFromExecutingAssemblyFolderFiles()
        {
            List<Type> types = new List<Type>();

            Assembly assembly = GetExecutingAssembly();
            types.AddRange(RetrieveTypesFromAssembly(assembly));

            return types.ToArray();
        }

        /// <summary>
        ///     This method is responsible to retrieve all types from the executing assembly folder by a specified
        ///     interface.
        /// </summary>
        /// <param name="interface">The interface that all returned types should inherit.</param>
        /// <returns>This method returns a collection of types that inherit the specified interface.</returns>
        public Type[] RetrieveTypesFromExecutingAssemblyFolderFolderByInterface(Type @interface)
        {
            Type[] typesFromAssembly = RetrieveTypesFromExecutingAssemblyFolderFiles();
            List<Type> typesWithInterface = new List<Type>();
            foreach(Type type in typesFromAssembly)
            {
                if(type.GetInterface("IDataSourceAdaptee") != @interface) continue;
                typesWithInterface.Add(type);
            }

            return typesWithInterface.ToArray();
        }

    }

}
