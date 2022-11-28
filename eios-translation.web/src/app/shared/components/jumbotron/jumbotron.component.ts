import { Component, Input, AfterViewInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'eios-jumbotron',
  templateUrl: './jumbotron.component.html',
  styleUrls: ['./jumbotron.component.css']
})
export class JumbotronComponent implements AfterViewInit {

  @Input() pageName?: string; 
  @Input() showSortingBtn?: boolean = false;
  @Input() showTimeFrameBtn?: boolean = false;
  @Input() subTitle: string;

  @Output() jumbotronSize: EventEmitter<{width: number, height: number}> = new EventEmitter<{width: number, height: number}>();

  constructor(){
  }
  ngOnInit() {
  }
  ngAfterViewInit() {
    const width: number = (<HTMLDivElement>document.getElementById("jumbotron")).offsetWidth;
    const height: number = (<HTMLDivElement>document.getElementById("jumbotron")).offsetHeight;
    this.jumbotronSize.emit({width, height});
  }
}
