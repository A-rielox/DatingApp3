import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
   selector: 'app-roles-modal',
   templateUrl: './roles-modal.component.html',
   styleUrls: ['./roles-modal.component.css'],
})
export class RolesModalComponent implements OnInit {
   username = '';
   availableRoles: any[] = [];
   selectedRoles: any[] = [];

   constructor(public bsModalRef: BsModalRef) {}

   ngOnInit(): void {}

   updateChecked(checkedValue: string) {
      const index = this.selectedRoles.indexOf(checkedValue);

      // si NO esta en la lista de roles q se le manda => se añade
      // cuando SI está ( !== -1 ) se se quita
      index !== -1
         ? this.selectedRoles.splice(index, 1)
         : this.selectedRoles.push(checkedValue);
   }
}
