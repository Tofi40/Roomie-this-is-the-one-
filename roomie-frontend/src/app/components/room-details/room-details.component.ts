import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Room } from '../../models/room.model';
import { RoomPhoto } from '../../models/room-photo.model';
import { RoomService } from '../../services/room.service';

@Component({
  selector: 'app-room-details',
  templateUrl: './room-details.component.html',
  styleUrls: ['./room-details.component.css']
})
export class RoomDetailsComponent implements OnInit {
  room?: Room;
  photos: RoomPhoto[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private roomService: RoomService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (!id) {
      this.errorMessage = 'Missing room id in route.';
      this.isLoading = false;
      return;
    }

    this.roomService.getRoomById(id).subscribe({
      next: room => {
        this.room = room;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Could not load this room.';
        this.isLoading = false;
      }
    });

    this.roomService.getPhotosByRoomId(id).subscribe({
      next: photos => (this.photos = photos),
      error: () => (this.photos = [])
    });
  }
}
