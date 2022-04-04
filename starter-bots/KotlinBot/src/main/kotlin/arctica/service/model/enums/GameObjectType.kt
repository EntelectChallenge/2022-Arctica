package arctica.service.model.enums


enum class GameObjectType(val value: Int) {
    ERROR(0),
    PLAYER_BASE(1),
    SCOUT_TOWER(2),
    RESOURCE_NODE(3);


    companion object {
        fun valueOf(value: Int): GameObjectType = values().first { it.value == value }
    }
}
