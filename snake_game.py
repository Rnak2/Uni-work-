#!/usr/bin/env python3
import curses
import random
import time

def main(stdscr):
    # Initialize curses
    curses.curs_set(0)  # Hide cursor
    curses.start_color()
    curses.init_pair(1, curses.COLOR_GREEN, curses.COLOR_BLACK)  # Snake color
    curses.init_pair(2, curses.COLOR_RED, curses.COLOR_BLACK)    # Food color
    curses.init_pair(3, curses.COLOR_CYAN, curses.COLOR_BLACK)   # UI color
    
    # Get terminal dimensions
    height, width = stdscr.getmaxyx()
    
    # Check if terminal is large enough
    MIN_HEIGHT, MIN_WIDTH = 20, 60
    if height < MIN_HEIGHT or width < MIN_WIDTH:
        stdscr.clear()
        error_msg = f"Terminal too small! Min size: {MIN_WIDTH}x{MIN_HEIGHT}"
        if height > 5 and width > len(error_msg):
            stdscr.addstr(height // 2, max(0, (width - len(error_msg)) // 2), error_msg)
            stdscr.addstr(height // 2 + 1, max(0, (width - 21) // 2), "Press any key to exit.")
            stdscr.refresh()
            stdscr.getch()
        return
    
    # Create game window with safe dimensions
    game_height = min(height - 2, height - 4)
    game_width = min(width - 2, width - 4)
    game_win = curses.newwin(game_height, game_width, 1, 1)
    game_win.keypad(True)  # Enable special keys
    game_win.timeout(100)  # Refresh rate (ms)
    
    # Set up game variables
    snake_x = min(game_width // 4, game_width - 5)
    snake_y = game_height // 2
    snake = [
        [snake_y, snake_x],
        [snake_y, snake_x - 1],
        [snake_y, snake_x - 2]
    ]
    
    # Initial food position (with boundary checks)
    food_y = min(game_height // 2, game_height - 2)
    food_x = min(game_width // 2, game_width - 2)
    food = [food_y, food_x]
    
    # Safe boundary check before adding character
    if 0 <= food[0] < game_height and 0 <= food[1] < game_width:
        game_win.addch(food[0], food[1], curses.ACS_DIAMOND, curses.color_pair(2))
    
    # Initial direction (RIGHT)
    key = curses.KEY_RIGHT
    
    # Score
    score = 0
    
    # Game loop
    while True:
        # Draw border
        stdscr.clear()
        stdscr.border(0)
        
        # Show instructions (with safe positioning)
        title = " SNAKE GAME "
        score_text = f" Score: {score} "
        quit_text = " Press 'q' to quit "
        
        # Safely position text to prevent boundary errors
        stdscr.addstr(0, min(2, width - len(title) - 1), title, curses.color_pair(3))
        stdscr.addstr(height - 1, min(2, width - len(score_text) - 1), score_text, curses.color_pair(3))
        
        # Only show quit text if there's enough space
        if width > len(quit_text) + 5:
            quit_pos = max(len(score_text) + 5, min(width - len(quit_text) - 1, width - 17))
            stdscr.addstr(height - 1, quit_pos, quit_text, curses.color_pair(3))
        
        stdscr.refresh()
        
        # Get next key (non-blocking)
        next_key = game_win.getch()
        
        # Handle user input
        key = key if next_key == -1 else next_key
        
        # Check if user wants to quit
        if key == ord('q'):
            break
        
        # Determine new direction
        if key == curses.KEY_DOWN and not (key == curses.KEY_UP and len(snake) > 1):
            # Going down, can't directly go up if snake length > 1
            direction = [1, 0]
        elif key == curses.KEY_UP and not (key == curses.KEY_DOWN and len(snake) > 1):
            # Going up, can't directly go down if snake length > 1
            direction = [-1, 0]
        elif key == curses.KEY_LEFT and not (key == curses.KEY_RIGHT and len(snake) > 1):
            # Going left, can't directly go right if snake length > 1
            direction = [0, -1]
        elif key == curses.KEY_RIGHT and not (key == curses.KEY_LEFT and len(snake) > 1):
            # Going right, can't directly go left if snake length > 1
            direction = [0, 1]
        else:
            # Maintain previous direction if invalid key
            if key == curses.KEY_DOWN:
                direction = [1, 0]
            elif key == curses.KEY_UP:
                direction = [-1, 0]
            elif key == curses.KEY_LEFT:
                direction = [0, -1]
            else:  # Default: KEY_RIGHT
                direction = [0, 1]
        
        # Calculate new head position
        new_head = [snake[0][0] + direction[0], snake[0][1] + direction[1]]
        snake.insert(0, new_head)
        
        # Check for collisions with wall (using game_height and game_width)
        if (new_head[0] >= game_height or new_head[0] < 0 or 
            new_head[1] >= game_width or new_head[1] < 0):
            # Game over when hitting wall
            break
        
        # Check for collisions with self
        if new_head in snake[1:]:
            # Game over when snake hits itself
            break
        
        # Check if snake got food
        if snake[0] == food:
            # Generate new food (with safe boundaries)
            attempts = 0
            max_attempts = 100  # Prevent infinite loops
            while True:
                food = [
                    random.randint(1, game_height - 2),
                    random.randint(1, game_width - 2)
                ]
                attempts += 1
                if food not in snake or attempts >= max_attempts:
                    break
            
            # Safe check before adding character
            if 0 <= food[0] < game_height and 0 <= food[1] < game_width:
                game_win.addch(food[0], food[1], curses.ACS_DIAMOND, curses.color_pair(2))
            score += 10
        else:
            # Remove tail if no food eaten
            tail = snake.pop()
            game_win.addch(tail[0], tail[1], ' ')
        
        # Draw snake head (with boundary check)
        if 0 <= snake[0][0] < game_height and 0 <= snake[0][1] < game_width:
            game_win.addch(snake[0][0], snake[0][1], curses.ACS_CKBOARD, curses.color_pair(1))
        
        # Redraw food (with boundary check)
        if 0 <= food[0] < game_height and 0 <= food[1] < game_width:
            game_win.addch(food[0], food[1], curses.ACS_DIAMOND, curses.color_pair(2))
    
    # Game over
    stdscr.clear()
    game_over_msg = "GAME OVER!"
    score_msg = f"Your score: {score}"
    exit_msg = "Press any key to exit..."
    
    # Safe text positioning for game over screen
    game_over_y = max(0, min(height // 2 - 1, height - 3))
    score_y = max(0, min(height // 2, height - 2))
    exit_y = max(0, min(height // 2 + 1, height - 1))
    
    game_over_x = max(0, min((width - len(game_over_msg)) // 2, width - len(game_over_msg)))
    score_x = max(0, min((width - len(score_msg)) // 2, width - len(score_msg)))
    exit_x = max(0, min((width - len(exit_msg)) // 2, width - len(exit_msg)))
    
    # Add strings with safe positioning
    stdscr.addstr(game_over_y, game_over_x, game_over_msg, curses.color_pair(3))
    stdscr.addstr(score_y, score_x, score_msg, curses.color_pair(3))
    stdscr.addstr(exit_y, exit_x, exit_msg, curses.color_pair(3))
    
    stdscr.refresh()
    stdscr.getch()  # Wait for key press before exiting

if __name__ == "__main__":
    try:
        # Start the curses application
        curses.wrapper(main)
    except KeyboardInterrupt:
        # Handle Ctrl+C gracefully
        pass
    except curses.error as e:
        # Handle curses-related errors (like terminal size issues)
        print(f"Terminal error: {e}")
        print("Try resizing your terminal to a larger size (minimum 60x20)")
    finally:
        print("Thanks for playing Snake!")

