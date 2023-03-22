import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {
   NgxGalleryAnimation,
   NgxGalleryImage,
   NgxGalleryOptions,
} from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { Member } from 'src/app/_models/member';
import { Message } from 'src/app/_models/message';
import { MessageService } from 'src/app/_services/message.service';

@Component({
   selector: 'app-member-detail',
   templateUrl: './member-detail.component.html',
   styleUrls: ['./member-detail.component.css'],
})
export class MemberDetailComponent implements OnInit {
   @ViewChild('memberTabs', { static: true }) memberTabs?: TabsetComponent;
   activeTab?: TabDirective;

   member: Member = {} as Member;
   messages: Message[] = [];
   galleryOptions: NgxGalleryOptions[] = [];
   galleryImages: NgxGalleryImage[] = [];

   constructor(
      private route: ActivatedRoute,
      private messageService: MessageService
   ) {}

   ngOnInit(): void {
      // AGARRO EL MEMBER DEL RESOLVER
      this.route.data.subscribe({
         next: (data) => (this.member = data['member']),
      });

      this.route.queryParams.subscribe({
         next: (params) => {
            if (params['tab']) {
               this.selectTab(params['tab']);
            }

            // params['tab'] && this.selectTab(params['tab']);
         },
      });

      this.galleryOptions = [
         {
            width: '500px',
            height: '500px',
            imagePercent: 100,
            thumbnailsColumns: 4,
            imageAnimation: NgxGalleryAnimation.Slide,
            preview: false,
         },
      ];

      this.galleryImages = this.getImages();
   }

   getImages() {
      if (!this.member) return [];

      const imageUrls = [];

      for (const photo of this.member.photos) {
         imageUrls.push({
            small: photo.url,
            medium: photo.url,
            big: photo.url,
         });
      }

      return imageUrls;
   }

   selectTab(heading: string) {
      if (this.memberTabs) {
         this.memberTabs.tabs.find((t) => t.heading === heading)!.active = true;
      }
   }

   loadMessages() {
      if (this.member) {
         this.messageService.getMessageThread(this.member.userName).subscribe({
            next: (messages) => (this.messages = messages),
         });
      }
   }

   onTabActivated(data: TabDirective) {
      this.activeTab = data;

      if (this.activeTab.heading === 'Messages') {
         this.loadMessages();
      }
   }
}
