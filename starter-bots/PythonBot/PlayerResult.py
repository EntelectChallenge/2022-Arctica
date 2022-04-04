class PlayerResult:
    def __init__(self, player_id, nickname, score, seed, placement, match_points) -> None:
        self.id = player_id
        self.nickname = nickname
        self.score = score
        self.seed = seed
        self.placement = placement
        self.match_points = match_points