
export class KindOfSport {
    public id: number;
    public sport: string;

    constructor(
        sport: string,
        id?: number
    ) {
        this.sport = sport;
        this.id = id;
    }
}