import enum 
  
class ResourceTypes(enum.Enum):
    Error = 0
    Wood = 1
    Food = 2
    Stone = 3
    Gold = 4
    Heat = 5

    def __str__(self) -> str:
        return '%s' % self.name