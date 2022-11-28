import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class uiJumbotronService{

    height: number;
    width: number;

    constructor() {
    }

    setHeight(heightInput: number, windowWidth: number): void {
        let heightVal = windowWidth - heightInput - 94 - 48;
        this.changeHeight(heightVal)
    }
    changeHeight(heightVal: number){
        this.height = heightVal;
    }
  
}