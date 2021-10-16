using System.Collections.Generic;
using UnityEngine;


namespace Rescues
{
    public sealed class InitializeNotepadController : IInitializeController
    {
        private readonly GameContext _context;
        private NotepadBehaviour _notepadBehaviour;

        public InitializeNotepadController(GameContext context)
        {
            _context = context;
        }

        public void Initialize()
        {           
            _notepadBehaviour = Object.FindObjectOfType<NotepadBehaviour>(true); 
            _context.notepad = _notepadBehaviour;
        }
    }
}
