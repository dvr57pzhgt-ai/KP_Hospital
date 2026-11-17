using KP_Hospital.Models;
using System.Collections.Generic;

namespace KP_Hospital.Services
{
    /// <summary>
    /// Паттерн "Снимок" (Caretaker). 
    /// Хранит историю снимков для функции "Отменить действие".
    /// </summary>
    public class MedicalRecordCaretaker
    {
        private Stack<MedicalRecordMemento> _history = new Stack<MedicalRecordMemento>();

        /// <summary>
        /// Сохраняет снимок в историю.
        /// </summary>
        public void Save(MedicalRecord record)
        {
            _history.Push(record.SaveState());
        }

        /// <summary>
        /// Восстанавливает последнее состояние.
        /// </summary>
        
        public bool Undo(MedicalRecord record)
        {
            if (_history.Count > 0)
            {
                var memento = _history.Pop();
                record.RestoreState(memento);
                return true;
            }
            return false;
        }
    }
}