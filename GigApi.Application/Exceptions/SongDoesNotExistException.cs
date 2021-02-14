using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApi.Application.Exceptions
{
    public class SongDoesNotExistException : Exception
    {
        public SongDoesNotExistException(Guid songId) : base($"Es existiert kein Song mit der Id \"{songId}\".")
        {
        }
    }
}
