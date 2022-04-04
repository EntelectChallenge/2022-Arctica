import enum

class GameObjects(enum.Enum):
    Error = 0
    Player_Base = 1
    Scout_Tower = 2
    Resource_Node = 3

    def __str__(self) -> str:
        return '%s' % self.name