import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { MembersService } from 'src/app/_services/members.service';

@Component({
   selector: 'app-member-list',
   templateUrl: './member-list.component.html',
   styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
   members: Member[] = [];
   pagination: Pagination | undefined;
   pageNumber = 1;
   pageSize = 5;

   constructor(private memberService: MembersService) {}

   ngOnInit(): void {
      this.loadMembers();
   }

   loadMembers() {
      this.memberService.getMembers(this.pageNumber, this.pageSize).subscribe({
         next: (paginatedRes) => {
            if (paginatedRes.result && paginatedRes.pagination) {
               this.members = paginatedRes.result;
               this.pagination = paginatedRes.pagination;
            }
         },
      });
   }

   pageChanged(e: any) {
      // if (this.userParams && this.userParams?.pageNumber !== e.page) {
      //    this.userParams.pageNumber = e.page;

      // para que tambien cambie en el memberService q es donde esta "respaldado" p' el caso en q se cambia de pagina
      // this.memberService.setUserParams(this.userParams);

      if (this.pageNumber !== e.page) {
         this.pageNumber = e.page;
         this.loadMembers();
      }
   }
}
