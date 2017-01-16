export class App {
    
    private counterValue: number = 5;

    get counter() {
        return this.counterValue;
    }

    set counter(val) {
        this.counterValue = val;
    }

    decrement() {
        this.counter--;
    }

    increment() {
        this.counter++;
    }
}