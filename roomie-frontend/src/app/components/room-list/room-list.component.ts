import { Component, OnInit } from '@angular/core';
import { Room } from '../../models/room.model';
import { RoomService } from '../../services/room.service';

@Component({
  selector: 'app-room-list',
  templateUrl: './room-list.component.html',
  styleUrls: ['./room-list.component.css']
})
export class RoomListComponent implements OnInit {
  rooms: Room[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(private roomService: RoomService) {}

  ngOnInit(): void {
    this.roomService.getAllRooms().subscribe({
      next: rooms => {
        this.rooms = rooms;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Could not load rooms. Please try again later.';
        this.isLoading = false;
      }
    });
  }
}
