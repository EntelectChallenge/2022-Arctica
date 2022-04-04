package arctica.service.model.enums

enum class ActionType(val value: Int) {
    ERROR(0),
    SCOUT(1),
    MINE(2),
    FARM(3),
    LUMBER(4);

    override fun toString(): String {
        return this.name
    }

    companion object {
        fun getRandomActionType(): ActionType = values().random()

    }
}
