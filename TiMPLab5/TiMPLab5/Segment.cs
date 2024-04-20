

namespace Task1
{
    /// <summary>
    /// Отрезок
    /// </summary>
    public class Segment
    {
        private int _beginPoint;
        private int _endPoint;
        private int _length;
        public int BeginPoint => _beginPoint;   
        public int EndPoint => _endPoint; 
        public int Length => _length;
            
        public Segment(int beginPoint, int endPoint)
        {
            if(beginPoint <= endPoint)
            {
                _beginPoint = beginPoint;
                _endPoint = endPoint;
            }
            else
            {
                _beginPoint = endPoint;
                _endPoint = beginPoint;
            }
            
            _length = _endPoint - _beginPoint;
        }


    }
}
