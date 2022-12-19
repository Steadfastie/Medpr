import { Component, OnInit } from '@angular/core';
import { Feed } from 'src/app/models/feed';
import { FeedService } from 'src/app/services/feed/feed.service';

import { inAnimation } from 'src/app/modules/animations/animations';
import { trigger, transition, useAnimation } from "@angular/animations";

@Component({
  selector: 'home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  animations: [
    trigger('insert', [
      transition(':enter', [
        useAnimation(inAnimation)
      ])
    ]),
  ]
})
export class HomeComponent implements OnInit{
  upcomingFeed?: Feed;
  ongoingFeed?: Feed;

  constructor(private feedService: FeedService) { }
  ngOnInit(): void {
    this.feedService.getUpcoming().subscribe((upcomingFeed: Feed) => {
      this.upcomingFeed = upcomingFeed;
    })

    this.feedService.getOngoing().subscribe((ongoingFeed: Feed) => {
      this.ongoingFeed = ongoingFeed;
    })

  }
}
