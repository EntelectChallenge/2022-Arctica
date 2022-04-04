package arctica.service.model

import java.util.UUID

open class GameObject(
    open val id: UUID,
    open val gameObjectType: Int,
    open val position: Position
)