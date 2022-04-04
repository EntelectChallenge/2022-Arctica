import enum
from msilib.schema import Error 
  
class ActionTypes(enum.Enum):
    Error = 0
    Scout = 1
    Mine = 2
    Farm = 3
    Lumber = 4

    def __str__(self):
        return '%s' % self.name