import { ChangeDetectionStrategy, Component, Input, OnInit, Pipe } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ErrorComponent implements OnInit {
  @Input() message!: string;

  constructor(private router: Router,
    private location: Location) { }

  ngOnInit(): void {
  }

  reloadPage(){
    window.location.reload();
  }

  back(){
    this.location.back();
  }
}
