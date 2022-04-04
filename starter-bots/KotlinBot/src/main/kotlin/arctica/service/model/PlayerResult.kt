package arctica.service.model

data class PlayerResult(
	val placement: Int,
	val seed: Int,
	val score: Int,
	val id: String,
	val nickname: String,
	val matchPoints: Int
)