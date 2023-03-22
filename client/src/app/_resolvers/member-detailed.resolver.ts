import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { Member } from '../_models/member';
import { MembersService } from '../_services/members.service';

@Injectable({
   providedIn: 'root',
})
export class MemberDetailedResolver implements Resolve<Member> {
   constructor(private memberService: MembersService) {}

   resolve(route: ActivatedRouteSnapshot): Observable<Member> {
      const memName = route.paramMap.get('username');

      return this.memberService.getMember(memName!);
   }
}
