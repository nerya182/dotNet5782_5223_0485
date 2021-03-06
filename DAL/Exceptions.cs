using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class ItemAlreadyExistsException : Exception
    {
        public int Id;
        public ItemAlreadyExistsException(int obj_Id) : base()
        {
            Id = obj_Id;
        }
        public ItemAlreadyExistsException(int obj_Id, String message) : base(message)
        {
            Id = obj_Id;
        }
        public ItemAlreadyExistsException(int obj_Id, String message, Exception inner) : base(message, inner)
        {
            Id = obj_Id;
        }
        public override string ToString()
        {
            return "Item with ID: " + Id + " already exists in data!\n" + Message;
        }
    }

    public class ItemNotFoundException : Exception
    {
        public int Id;
        public ItemNotFoundException(int obj_Id) : base()
        {
            Id = obj_Id;
        }
        public ItemNotFoundException(int obj_Id, String message) : base(message)
        {
            Id = obj_Id;
        }
        public ItemNotFoundException(int obj_Id, String message, Exception inner) : base(message, inner)
        {
            Id = obj_Id;
        }
        public override string ToString()
        {
            return "Item with ID: " + Id + " was not found in data!\n" + Message;
        }
    }

    public class IllegalActionException : Exception
    {
        public IllegalActionException() : base("") { }

        public IllegalActionException(String message) : base(message) { }

        public IllegalActionException(String message, Exception inner) : base(message, inner) { }

        public override string ToString()
        {
            return "Illegal Action has been attempted!\n" + Message;
        }
    }
}
