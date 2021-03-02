using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApi.Application.Exceptions
{
    public class SongMultipleTimesInPlaylistException : Exception
    {
        public SongMultipleTimesInPlaylistException(Guid songId) : base($"Der Song mit der Id \"{songId}\" kann nicht mehrmals zu einer Playlist hinzugefügt werden.")
        {
        }
    }
}
