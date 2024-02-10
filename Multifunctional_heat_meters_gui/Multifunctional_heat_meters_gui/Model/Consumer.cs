using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui.Model
{
    public class Consumer
    {
        private bool _active;

        private int _id;

        private int _accountingSchemeNumber;

        private int _pipelinesCount;

        public enum PipelineStatus
        {
            NOT_USED,
            SUPPLYING,
            REVERSE,
            RECHARGE
        }

        private List<PipelineStatus> _pipelinesStatuses;

        //Является ли потребитель активным
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        //Идентификатор потребителя
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        //Схема учета
        public int AccountingSchemeNumber
        {
            get { return _accountingSchemeNumber; }
            set { _accountingSchemeNumber = value; }
        }

        //Количество трубопроводов
        public int PipelinesCount
        {
            get { return _pipelinesCount; }
            set
            {
                if (value > _pipelinesStatuses.Count)
                {
                    for (int i = 0; i < value - _pipelinesCount; i++)
                    {
                        _pipelinesStatuses.Add(PipelineStatus.NOT_USED);
                    }
                }
                _pipelinesCount = value;
            }
        }

        public Consumer(int pipelinesCount, bool active = false, int id = 0, int schemeNumber = 0)
        {
            _active = active;
            _id = id;
            _accountingSchemeNumber = schemeNumber;
            _pipelinesCount = pipelinesCount;
            _pipelinesStatuses = new List<PipelineStatus>();
            for (int i = 0; i < pipelinesCount; i++)
            {
                _pipelinesStatuses.Add(PipelineStatus.NOT_USED);
            }
        }

        public void SetPipelineStatusByInd(int index, PipelineStatus status)
        {
            _pipelinesStatuses[index] = status;
        }

        public PipelineStatus GetPipelineStatusByInd(int index)
        {
            return _pipelinesStatuses[index];
        }
    }
}
