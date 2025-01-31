using System.Collections;
using System.Collections.Generic;
using ZX.Util;

namespace ThreeWeeks
{
    internal class FurnitureEnumerator : IEnumerator<CodeArgs>
    {
        #region Fields
        private int _index;
        private CodeArgs _current = null!;
        #endregion

        #region Construction
        public FurnitureEnumerator(int startOffset, 
            FurnitureDataGroup dataGroup)
        {
            StartOffset = startOffset;
            DataGroup = dataGroup;
            _current = null;
            (this as IEnumerator).Reset();
        }

        public void Dispose()
        {
            _current = null;
        }
        #endregion

        #region Properties
        private int StartOffset { get; }

        private FurnitureDataGroup DataGroup { get; }

        public CodeArgs Current => _current;

        object IEnumerator.Current => _current;
        #endregion

        #region IEnumerator
        bool IEnumerator.MoveNext()
        {
            _current = null;

            byte code = CurrentCode();

            bool valid = code != (byte)FurnitureCode.Exit;

            if(valid)
            {
                _current = CreateArgs(code);

                if(DataGroup.SpecialCodes.TryGetValue(code, out EnumeratorAlterMethod alterMethod))
                {
                    _index = alterMethod(_index, DataGroup);
                }
                else
                {
                    _index += _current.Info.NumberOfArgs;
                }
            }

            return valid;
        }

        void IEnumerator.Reset()
        {
            _index = StartOffset;
            _current = CreateArgs(CurrentCode());
        }
        #endregion

        #region Private Helpers
        private byte CurrentCode()
        {
            return CheckForRangeCode(DataGroup.FurnitureData[_index]);
        }

        private CodeArgs CreateArgs(byte code)
        {
            CodeInfo info = DataGroup.CodeInfoMapping[code];

            return new CodeArgs(
                DataGroup.FurnitureData.CopyRange(_index, info.NumberOfArgs),
                info);
        }
 
         private byte CheckForRangeCode(byte code)
        {
            System.Diagnostics.Debug.Assert(DataGroup.CodeRanges != null, "Ranges should contain stuff");
            foreach(KUtil.Range range in DataGroup.CodeRanges)
            {
                if(range.Within(code))
                {
                    code = range.LessThan;
                    break;
                }
            }

            return code;
        }
       #endregion
    }
}