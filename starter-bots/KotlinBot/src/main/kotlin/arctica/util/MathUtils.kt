package arctica.util

import arctica.service.model.Position
import kotlin.math.*

object MathUtils {

	/** @return the distance between position a and b calculated using the Pythagorean theorem. */
	fun getDistanceBetween(a: Position, b: Position): Double {
		val x = abs(a.x - b.x)
		val y = abs(a.y - b.y)
		return pythagoras(x.toDouble(), y.toDouble())
	}

	private fun pythagoras(x: Double, y: Double): Double = sqrt(x * x + y * y)
}