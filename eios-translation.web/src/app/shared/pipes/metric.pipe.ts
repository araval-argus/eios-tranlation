import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'metric'
})
export class MetricPipe implements PipeTransform {

  transform(number: number, digits?: number): string {
    

    if (number < 0){ 
      return '0'; 
    }
    if (number < 1e3){ 
      return number.toString(); 
    }
    if (!digits){
        digits = 3;
    }
    
    const exp = Math.floor(Math.log(number) / Math.log(1e3));
    number = number / Math.pow(1e3, exp);
    const exp2 = Math.ceil(Math.log(number) / Math.log(1e1));
    
    if (digits < exp2){ 
      digits = exp2;
    }
    
    return (+number.toPrecision(digits) + 'kMGTPE'.charAt(exp - 1)).toString();
  }

}
