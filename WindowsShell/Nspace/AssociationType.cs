using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsShell.Nspace
{
    public enum AssociationType
    {
        /// <summary>
        /// No server association.
        /// </summary>
        None,

        /// <summary>
        /// Create an association to a specific file extension.
        /// </summary>
        FileExtension,

        /// <summary>
        /// Create an association to the class of a specific file extension.
        /// </summary>
        ClassOfExtension,

        /// <summary>
        /// Create an association to a class.
        /// </summary>
        Class,

        /// <summary>
        /// Create an association to the all files class.
        /// </summary>
        AllFiles,

        /// <summary>
        /// Create an association to the directory class.
        /// </summary>
        Directory,

        /// <summary>
        /// Create an association to the drive class.
        /// </summary>
        Drive,

        /// <summary>
        /// Create an association to the unknown files class.
        /// </summary>
        UnknownFiles
    }
}
