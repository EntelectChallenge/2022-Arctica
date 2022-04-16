import enum
  
class ActionTypes(enum.Enum):
    Error = 0
    Scout = 1
    Mine = 2
    Farm = 3
    Lumber = 4
    StarCampfire = 5

    def __str__(self) -> str:
        return '%s' % self.name