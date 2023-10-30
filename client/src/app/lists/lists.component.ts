import { Component, OnInit } from '@angular/core';
import { Member } from '../_models/member';
import { MembersService } from '../_services/members.service';
import { Pagination } from '../_models/pagination';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  members: Member[] | undefined;
  predicate = "liked";
  pageNumber = 1;
  pageSize = 30;
  pagination : Pagination | undefined;

  constructor(private membeService: MembersService){}
  ngOnInit(): void {
    this.loadLikes();
  }

  loadLikes()
  {
    this.membeService.getLike(this.predicate, this.pageNumber,this.pageSize).subscribe({
      next: repsonse =>
      {
        this.members = repsonse.result;
        this.pagination = repsonse.pagination;
      }
    })
  }

  pageChanged(event: any) {
    if (this.pageNumber !== event.page)
     {
      this.pageNumber = event.page
      this.loadLikes();
    }
  }

}
