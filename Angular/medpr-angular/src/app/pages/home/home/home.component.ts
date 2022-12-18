import { Component, OnInit } from '@angular/core';
import { Feed } from 'src/app/models/feed';
import { FeedService } from 'src/app/services/feed/feed.service';

@Component({
  selector: 'home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
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
