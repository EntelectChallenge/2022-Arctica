import enum 
  
class ResouceTypes(enum.Enum):
    Error = 0
    Wood = 1
    Food = 2
    Stone = 3
    Gold = 4

    def __str__(self) -> str:
        return '%s' % self.name