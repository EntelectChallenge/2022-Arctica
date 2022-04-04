class NodeService {
    constructor(bot) {
        this.bot = bot;
        this.nodeDistances = {};
        this.visitedScoutTowers = [];
    }

    distance = (a, b) => {
        const deltaX = a.x - b.x;
        const deltaY = a.y - b.y;
        return Math.sqrt(deltaX**2 + deltaY**2);
    }

    computeAndStoreLocation = (node) => {
        const distance = this.distance(node.position, this.bot.baseLocation)
        this.nodeDistances[node.id] = distance;
        return distance;
    }

    getNodeDistance = (node) => {
        if(!node) {
            return Infinity;
        }
        return this.nodeDistances[node.id] ?? this.computeAndStoreLocation(node);
    }

    getClosest = (nodes) => {
        return nodes.reduce((prev, node) => this.getNodeDistance(node) < this.getNodeDistance(prev) ? node : prev, null);
    }

    getNearestUnvisitedScoutTower = () => {
        const unvisitedScoutTowers = this.bot.myScoutTowers.filter(tower => !this.visitedScoutTowers.includes(tower.id));
        const nearestScoutTower = this.getClosest(unvisitedScoutTowers);
        this.visitedScoutTowers.push(nearestScoutTower.id);
        return nearestScoutTower;
    }

    getNodesByProximity = () => {
        const nodesByProximity = [...this.bot.myNodes];
        nodesByProximity.sort(node => this.getNodeDistance(node));
        return nodesByProximity;
    }
    
    getNodesByType = (resourceType) => {
        return this.bot.myNodes.filter(node => node.type === resourceType);    
    }
}

module.exports = {NodeService};